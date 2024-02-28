##Тимошевич Карина, группа 253503##

###Мобильное приложение для системы книжного магазина###

1. **BaseEntity**:
   - **ID**: Свойство для хранения идентификатора сущности  (в будущем может понадобиться для работы с базой данных).

2. **PersonalInfo**:
   - **FirstName**: Имя пользователя.
   - **LastName**: Фамилия пользователя.
   - **Email**: Адрес электронной почты пользователя.
   - **PhoneNumber**: Номер телефона пользователя.
   - **UpdateInfo(string firstName, string lastName, string email, string phoneNumber)**: Метод для обновления информации о пользователе.

3. **Book**:
   - **ISBN**: ISBN книги.
   - **Title**: Название книги.
   - **Author**: Автор книги.
   - **Publisher**: Издатель книги.
   - **Year**: Год издания книги.
   - **Genre**: Жанр книги.
   - **Price**: Цена книги.
   - **QuantityInStock**: Количество книги в наличии на складе.
   - **Book(string isbn, string title, string author, string publisher, int year, string genre, decimal price)**: Конструктор для создания экземпляра книги.
   - **AddToInventory(int quantity)**: Метод для добавления книги на склад.
   - **RemoveFromInventory(int quantity)**: Метод для удаления книги со склада.

4. **User**:
   - **Username**: Имя пользователя.
   - **PersonalInfo**: Информация о пользователе.
   - **Password**: Пароль пользователя.
   - **ChangePassword(string newPassword)**: Метод для изменения пароля пользователя.
   - **UpdatePersonalInfo(string firstName, string lastName, string email, string phoneNumber)**: Метод для обновления информации о пользователе.

5. **Admin**:
   - Наследует все свойства и методы класса User.
   - **AddBook(List<Book> catalog, Book book)**: Метод для добавления книги в каталог.
   - **RemoveBook(List<Book> catalog, int bookID)**: Метод для удаления книги из каталога по её идентификатору.
   - **UpdateBookInfo(List<Book> catalog, int bookID, Book updatedBook)**: Метод для обновления информации о книге в каталоге.
   - **ViewOrders(List<Order> orders)**: Метод для просмотра списка заказов.
   - **ViewCustomers(List<Customer> customers)**: Метод для просмотра списка клиентов.
   - **AssignEmployee(Employee employee, string position)**: Метод для назначения должности сотруднику.
   - **FireEmployee(Employee employee)**: Метод для увольнения сотрудника.
   - **BanCustomer(Customer customer)**: Метод для блокировки клиента.

6. **Employee**:
   - Наследует все свойства и методы класса User.
   - **Salary**: Заработная плата сотрудника.
   - **Position**: Должность сотрудника.
   - **Promote(decimal newSalary, string newPosition)**: Метод для повышения сотрудника.

7. **Customer**:
   - Наследует все свойства и методы класса User.
   - **ShippingAddress**: Адрес доставки клиента.
   - **OrderHistory**: История заказов клиента.
   - **CurrentOrder**: Текущий заказ клиента.
   - **PlaceOrder()**: Метод для размещения заказа.
   - **AddBookToOrder(Book book, int quantity)**: Метод для добавления книги в текущий заказ.
   - **ViewCurrentOrder()**: Метод для просмотра текущего заказа.
   - **RemoveBookFromOrder(Book book, int quantity)**: Метод для удаления книги из текущего заказа.

8. **Order**:
   - **OrderDate**: Дата заказа.
   - **Status**: Статус заказа.
   - **TotalAmount**: Общая сумма заказа.
   - **OrderItems**: Список позиций заказа.
   - **CalculateTotalAmount()**: Метод для расчёта общей суммы заказа.
   - **AddOrderItem(OrderItem item)**: Метод для добавления позиции заказа.

9. **OrderItem**:
   - **BookID**: Идентификатор книги.
   - **Quantity**: Количество книг.
   - **Price**: Цена за единицу книги.

10. **Payment**:
    - Наследует все свойства и методы класса BaseEntity.
    - **Amount**: Сумма платежа.
    - **Date**: Дата платежа.
    - **Status**: Статус платежа.
    - **ProcessPayment(Order order)**: Метод для обработки платежа.

11. **Supplier**:
    - Наследует все свойства и методы класса BaseEntity.
    - **CompanyName**: Название компании поставщика.
    - **ContactPerson**: Контактное лицо.
    - **PhoneNumber**: Номер телефона поставщика.
    - **Email**: Адрес электронной почты поставщика.

12. **Inventory**:
    - **Books**: Список книг на складе.
    - **AddBook(Book book)**: Метод для добавления книги на склад.
    - **RemoveBook(Book book)**: Метод для удаления книги со склада.
    - **UpdateInventory(Book book, int quantity)**: Метод для обновления информации о количестве книг на складе.
    - **ViewQuantityOfBookInStock(Book book)**: Метод для просмотра количества книг на складе.

13. **Discount**:
    - Наследует все свойства и методы класса BaseEntity.
    - **Percentage**: Процент скидки.
    - **StartDate**: Начальная дата действия скидки.
    - **EndDate**: Конечная дата действия скидки.
    - **ApplyDiscount(Order order)**: Метод для применения скидки к заказу.

14. **BookshopApplication**:
    - **customers**: Список клиентов.
    - **admin**: Администратор.
    - **AddCustomer(Customer customer)**: Метод для добав

ления клиента.
    - **Register(string username, string password, PersonalInfo personalInfo, string phoneNumber)**: Метод для регистрации нового клиента.
    - **Login(string username, string password)**: Метод для входа в систему.
    - **Logout()**: Метод для выхода из системы.