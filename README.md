# Семестровая работа по ОРИС. Вебсайт с нейросетью.

Данное приложение представляет собой сайт с нейросетью, которая может писать текст по запорсу. Имеет регистрацию и уровни подписок которые открывают возможность писать запросы.

Текстовое задание сайта находится в файле тз.docs

Figma: https://www.figma.com/design/22m79Sm9LMSFR2DTCdYSmp/Untitled?node-id=0-1&t=2mJawBCu9aEAhjQR-1

## База данных
В проекте используется База данных PostgreSQL. Для подключения к своей бд нужно изменить conection string: 

"Db" : "Host=your_host;Port=your_port;Database=your_db_Name;Username=your_username;Password=your_password;Include Error Detail=true"

Сделать миграцию команды: 
1. dotnet ef migrations add initial_13 -s Eko.Host -p Eko.Database 
2. dotnet ef database update -s Eko.Host -p Eko.Database

## Развертывание нейросети
Для работы нейросети требуется развернуть image Ollama и загрузить deepseekR1:

ducker pull ollama/ollama что бы запустить на CPU  
docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama что бы запустить на GPU 
docker run -d --gpus=all -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama контейнер запускается по порту 11434

Ollama предоставляет широкий выбор обученных моделей, для примера можно использовать deepseekR1 : 8b 

docker exec ollama ollama run deepseek-r1:8b

Писать запросы могут только пользователи с подпиской Bronze и выше
## Административная панель
Доступ к административной панели имеют только пользователи с ролью admin в поле role. Задать это поле можно только изменив данные пользователя в бд, или создав пользователя через запрос в бд с такой ролью. Пользователи с этой ролью могут попасть на административную панель через профиль нажав на ссылку своей роли.   
