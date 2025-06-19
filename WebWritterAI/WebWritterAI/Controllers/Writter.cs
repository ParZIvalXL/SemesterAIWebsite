using System.Text;
using System.Text.Json;
using DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Services.Services;

namespace WebWritterAI.Controllers;

[Controller]
[Route("[controller]")]
public class Writter : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Writter> _logger;
    private readonly UserService _userService;
    private readonly ChatService _chatService;
    private readonly MessageService _messageService;
    
    public Writter(IHttpClientFactory httpClientFactory, 
                 IConfiguration configuration,
                 ILogger<Writter> logger, 
                 UserService userService,
                 ChatService chatService,
                 MessageService messageService)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _userService = userService;
        _chatService = chatService;
        _messageService = messageService;
    }

    [HttpGet("chat")]
    public async Task<IActionResult> ViewChat()
    {
        var redirectResult = await RedirectIfNotLogged();
        if(redirectResult is not OkResult) 
            return redirectResult;
        
        var userId = await _userService.GetUserByToken(User);
        if (userId == null) 
            return Redirect("/Pricing/subscriptions" );
        
        return View("chat");
    }
    
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] ChatRequest request)
    {
        try
        {
            _logger.LogInformation("AskDeepSeek");
            _logger.LogInformation("GetRequestAiQueryHandler.Execute");
            var result = await GetResponse(request.Message);
            return Json(new { 
                success = true,
                response = result 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Send method");
            return StatusCode(500, new { 
                success = false,
                error = ex.Message 
            });
        }
    }

    [HttpGet("chats")]
    public async Task<IActionResult> LoadChats()
    {
        try
        {
            var user = await _userService.GetUserByToken(User);
            if (user == null) 
                return Unauthorized(new { 
                    success = false,
                    error = "Unauthorized" 
                });
            
            var chats = await _chatService.GetUserChats(user.Value);
        
            // Убедимся, что возвращаем правильный формат
            return Ok(new {
                success = true,
                chats = chats // chats должен быть List<ChatModel> или аналогичный перечисляемый тип
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading chats");
            return StatusCode(500, new {
                success = false,
                error = ex.Message
            });
        }
    }
    
    [HttpPost("create-chat")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
    {
        try
        {
            var user = await _userService.GetUserByToken(User);
            if (user == null) 
                return Unauthorized(new { error = "Unauthorized" });

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { error = "Chat name cannot be empty" });

            var newChat = new ChatModel()
            {
                UserId = user.Value,
                Name = request.Name,
            };
        
            await _chatService.AddChat(newChat);
        
            return Ok(new 
            {
                success = true,
                chatId = newChat.Id,
                chatName = newChat.Name
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating chat");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("send-message/{chatId}")]
    public async Task<IActionResult> SendMessage(Guid chatId, [FromBody] SendMessageRequest request)
    {
        try
        {
            var user = await _userService.GetUserByToken(User);
            if (user == null) 
                return Unauthorized(new { error = "Unauthorized" });

            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { error = "Message cannot be empty" });

            var newMessage = new MessageModel()
            {
                ChatId = chatId,
                UserId = request.IsFromAI ? Guid.Empty : user.Value, 
                Message = request.Message,
            };
        
            await _messageService.AddMessage(newMessage);
        
            return Ok(new 
            {
                success = true,
                messageId = newMessage.Id
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending message to chat {chatId}");
            return StatusCode(500, new { error = ex.Message });
        }
    }
    
    [HttpGet("messages/{chatId}")]
    public async Task<IActionResult> LoadMessages(Guid chatId)
    {
        try
        {
            // Проверяем существование чата
            var chatExists = await _chatService.ChatExists(chatId);
            if (!chatExists)
            {
                return NotFound(new {
                    success = false,
                    error = "Chat not found"
                });
            }

            var messages = await _messageService.GetMessageByChatId(chatId);
        
            // Преобразуем в массив, если это не массив
            var messagesArray = messages as IEnumerable<MessageModel> ?? messages.ToArray();
        
            return Ok(new {
                success = true,
                messages = messagesArray.Select(m => new {
                    message = m.Message,
                    userId = m.UserId,
                    //isFromAI = m.IsFromAI,
                    //createdAt = m.CreatedAt
                }).ToArray() // Явно преобразуем в массив
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error loading messages for chat {chatId}");
            return StatusCode(500, new {
                success = false,
                error = ex.Message
            });
        }
    }
    
    private async Task<IActionResult> RedirectIfNotLogged()
    {
        var userId = await _userService.GetUserByToken(User);
        if (userId == null) 
            return Redirect("/auth/login");
            
        return Ok();
    }
    
    private async Task<string> GetResponse(string prompt)
    {
        var client = _httpClientFactory.CreateClient("Ollama");
        
        var requestBody = new
        {
            model = "deepseek-r1:8b",
            prompt = prompt,
            stream = false
        };

        var response = await client.PostAsJsonAsync("api/generate", requestBody);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(responseContent);
        if (jsonDoc.RootElement.TryGetProperty("response", out var responseElement))
        {
            _logger.LogInformation($"Response: {responseElement.GetString()}");
            return responseElement.GetString();
        }

        throw new Exception("Invalid response format from Ollama");
    }
}

// DTO классы
public class ChatRequest
{
    public string Message { get; set; }
}

public class CreateChatRequest
{
    public string Name { get; set; }
}

public class SendMessageRequest
{
    public string Message { get; set; }
    public bool IsFromAI { get; set; }
}