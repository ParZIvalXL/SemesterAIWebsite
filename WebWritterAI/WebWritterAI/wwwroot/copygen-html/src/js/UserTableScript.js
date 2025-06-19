// Объявляем переменные в глобальной области видимости
let currentPage = 0; // Начинаем с 0, так как сервер использует 0-based
const perPage = 10;
let totalUsers = 0; // Будем хранить общее количество пользователей

document.addEventListener('DOMContentLoaded', function() {
    console.log("Loaded");

    // Загружаем первую страницу
    loadUsers(currentPage);

    // Обработчики пагинации
    document.querySelector('.pagination .prev')?.addEventListener('click', () => {
        if (currentPage > 0) {
            currentPage--;
            loadUsers(currentPage);
        }
    });

    document.querySelector('.pagination .next')?.addEventListener('click', () => {
        // Проверяем, есть ли следующая страница
        if ((currentPage + 1) * perPage < totalUsers) {
            currentPage++;
            loadUsers(currentPage);
        }
    });

    // Обработчик поля ввода страницы
    document.querySelector('.pagination input')?.addEventListener('change', function() {
        const page = parseInt(this.value) - 1; // Преобразуем в 0-based
        const totalPages = Math.ceil(totalUsers / perPage);

        if (page >= 0 && page < totalPages) {
            currentPage = page;
            loadUsers(page);
        } else {
            this.value = currentPage + 1; // Восстанавливаем предыдущее значение (1-based)
        }
    });
});

async function loadUsers(page) {
    try {
        const response = await fetch(`/admin/users?page=${page}&perPage=${perPage}`);
        if (!response.ok) throw new Error('Server error');

        const users = await response.json();

        if (page === 0) {
            const countResponse = await fetch('/admin/users/count');
            totalUsers = await countResponse.json();
        }

        renderUsers(users);
        updatePagination();
    } catch (error) {
        showError(error);
    }
}

function renderUsers(users) {
    const tbody = document.querySelector('#users-table tbody');
    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    tbody.innerHTML = '';

    users.forEach(user => {
        const row = document.createElement('tr');
        row.dataset.userId = user.id;

        // Создаем ячейки таблицы
        const th = document.createElement('th');
        th.className = 'text-center';
        th.scope = 'row';
        th.innerHTML = '<input class="form-check-input" type="checkbox">';
        row.appendChild(th);

        const tdId = document.createElement('td');
        tdId.textContent = user.id;
        row.appendChild(tdId);

        const tdName = document.createElement('td');
        tdName.innerHTML = `<div class="d-flex align-items-center">${user.fullName}</div>`;
        row.appendChild(tdName);

        const tdRole = document.createElement('td');
        const roleClass = getRoleClass(user.role);
        tdRole.innerHTML = `<span class="${roleClass}">${user.role}</span>`;
        row.appendChild(tdRole);

        const tdEmail = document.createElement('td');
        tdEmail.textContent = user.email;
        row.appendChild(tdEmail);

        const tdAction = document.createElement('td');
        const deleteBtn = document.createElement('button');
        deleteBtn.className = 'btn btn-icon btn-sm btn-danger';
        deleteBtn.innerHTML = '<i class="ri-delete-bin-line"></i>';

        // Добавляем обработчик напрямую
        deleteBtn.addEventListener('click', () => {
            if (user.role === "admin") {
                showError("Нельзя удалить администратора");
                return;
            }
            deleteUser(user.id);
        });

        tdAction.appendChild(deleteBtn);
        row.appendChild(tdAction);

        tbody.appendChild(row);
    });

    // Обновляем счетчики
    const startCount = currentPage * perPage + 1;
    const endCount = Math.min((currentPage + 1) * perPage, totalUsers);
    const showingElement = document.querySelector('.showing');
    const totalElement = document.querySelector('.total');

    if (showingElement) showingElement.textContent = `${startCount}-${endCount}`;
    if (totalElement) totalElement.textContent = totalUsers;
}

function getRoleClass(role) {
    const roleStyles = {
        "admin": "badge bg-danger-transparent",
        "user": "badge bg-primary-transparent",
        "other": "badge bg-success-transparent"
    };
    return roleStyles[role] || roleStyles.other;
}

async function deleteUser(userId) {
    if (!confirm('Вы уверены, что хотите удалить пользователя?')) return;

    try {
        const response = await fetch(`/admin/users/${userId}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Delete failed');

        // После удаления перезагружаем текущую страницу
        totalUsers--; // Уменьшаем общее количество

        // Если текущая страница стала пустой и это не первая страница
        if (currentPage > 0 && currentPage * perPage >= totalUsers) {
            currentPage--;
        }

        loadUsers(currentPage);
        showSuccess('Пользователь удален');
    } catch (error) {
        showError(error);
    }
}

function updatePagination() {
    const totalPages = Math.ceil(totalUsers / perPage);
    const pageInfo = document.querySelector('.pagination .page-info');
    const pageInput = document.querySelector('.pagination input');
    const prevBtn = document.querySelector('.pagination .prev');
    const nextBtn = document.querySelector('.pagination .next');

    // Отображаем страницы как 1-based для пользователя
    if (pageInfo) pageInfo.textContent = `Страница ${currentPage + 1} из ${totalPages}`;
    if (pageInput) pageInput.value = currentPage + 1;
    if (prevBtn) prevBtn.classList.toggle('disabled', currentPage === 0);
    if (nextBtn) nextBtn.classList.toggle('disabled', (currentPage + 1) * perPage >= totalUsers);
}

function showSuccess(message) {
    console.log('Success:', message);
    alert(message);
}

function showError(error) {
    console.error('Error:', typeof error === 'string' ? error : error.message);
    alert(typeof error === 'string' ? error : error.message || 'Произошла ошибка');
}