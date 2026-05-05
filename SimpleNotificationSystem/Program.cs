using SimpleNotificationSystem.Models;
using SimpleNotificationSystem.Repositories;
using SimpleNotificationSystem.Services;

var userRepo = new UserRepository();
var notificationRepo = new NotificationRepository();

var emailChannel = new EmailNotification();
var smsChannel = new SmsNotification();
var whatsAppChannel = new WhatsAppNotification();
var notificationService = new NotificationService(emailChannel, smsChannel, whatsAppChannel);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("==== Simple Notification System (CRUD) ====");
    Console.WriteLine("User CRUD:");
    Console.WriteLine("  1. Create User");
    Console.WriteLine("  2. View All Users");
    Console.WriteLine("  3. View User By Email");
    Console.WriteLine("  4. Update User");
    Console.WriteLine("  5. Delete User");
    Console.WriteLine("Notification:");
    Console.WriteLine("  6. Create + Send Notification");
    Console.WriteLine("  7. View All Notifications");
    Console.WriteLine("  8. View Notification By Id");
    Console.WriteLine("  9. Update Notification");
    Console.WriteLine(" 10. Delete Notification");
    Console.WriteLine("  0. Exit");
    Console.Write("Enter choice: ");

    var choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            CreateUser(userRepo);
            break;
        case "2":
            ViewAllUsers(userRepo);
            break;
        case "3":
            ViewUser(userRepo);
            break;
        case "4":
            UpdateUser(userRepo);
            break;
        case "5":
            DeleteUser(userRepo);
            break;
        case "6":
            CreateAndSend(notificationService, userRepo, notificationRepo);
            break;
        case "7":
            ViewAllNotifications(notificationRepo);
            break;
        case "8":
            ViewNotification(notificationRepo);
            break;
        case "9":
            UpdateNotification(notificationRepo);
            break;
        case "10":
            DeleteNotification(notificationRepo);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Invalid choice. Try again.");
            break;
    }
}

static void CreateUser(UserRepository repo)
{
    var user = new User
    {
        Name = ReadNonEmpty("Name: "),
        Email = ReadNonEmpty("Email (acts as ID): "),
        PhoneNumber = ReadNonEmpty("Phone: ")
    };

    repo.Create(user);
    Console.WriteLine("User saved.");
    PrintUser(user);
}

static void ViewAllUsers(UserRepository repo)
{
    var users = repo.GetAll();
    if (users.Count == 0)
    {
        Console.WriteLine("No users found.");
        return;
    }

    foreach (var u in users.OrderBy(u => u.Email))
    {
        Console.WriteLine("-----------------------------");
        PrintUser(u);
    }
    Console.WriteLine("-----------------------------");
}

static void ViewUser(UserRepository repo)
{
    var email = ReadNonEmpty("Enter email: ");
    var user = repo.GetById(email);
    if (user is null)
    {
        Console.WriteLine("User not found.");
        return;
    }

    PrintUser(user);
}

static void UpdateUser(UserRepository repo)
{
    var email = ReadNonEmpty("Enter email to update: ");
    var user = repo.GetById(email);
    if (user is null)
    {
        Console.WriteLine("User not found.");
        return;
    }

    Console.WriteLine("Press ENTER to keep old value.");

    Console.WriteLine($"Current Name: {user.Name}");
    var name = ReadOptional("New Name: ");
    if (!string.IsNullOrWhiteSpace(name)) user.Name = name.Trim();

    Console.WriteLine($"Current Phone: {user.PhoneNumber}");
    var phone = ReadOptional("New Phone: ");
    if (!string.IsNullOrWhiteSpace(phone)) user.PhoneNumber = phone.Trim();

    repo.Update(email, user);
    Console.WriteLine("User updated.");
    PrintUser(user);
}

static void DeleteUser(UserRepository repo)
{
    var email = ReadNonEmpty("Enter email to delete: ");
    var deleted = repo.Delete(email);
    if (deleted is null)
    {
        Console.WriteLine("User not found.");
        return;
    }

    Console.WriteLine("User deleted:");
    PrintUser(deleted);
}

static void CreateAndSend(
    NotificationService notificationService,
    UserRepository userRepo,
    NotificationRepository notificationRepo)
{
    var email = ReadNonEmpty("Enter recipient user email: ");
    var user = userRepo.GetById(email);
    if (user is null)
    {
        Console.WriteLine("User not found. Create the user first.");
        return;
    }

    Console.WriteLine("Channel: 1 = Email, 2 = SMS, 3 = WhatsApp");
    var channel = ReadInt("Enter channel: ", min: 1, max: 3);
    var message = ReadNonEmpty("Message: ");

    var notification = new Notification { Message = message };
    var id = notificationRepo.CreateWithId(notification);

    switch (channel)
    {
        case 1:
            notificationService.SendEmail(user, notification);
            break;
        case 2:
            notificationService.SendSms(user, notification);
            break;
        case 3:
            notificationService.SendWhatsApp(user, notification);
            break;
    }

    Console.WriteLine($"Notification created + sent (Id = {id}, SentDate = {notification.SentDate}).");
}

static void ViewAllNotifications(NotificationRepository repo)
{
    var ids = repo.GetIds();
    if (ids.Count == 0)
    {
        Console.WriteLine("No notifications found.");
        return;
    }

    Console.WriteLine($"Total notifications: {ids.Count}");
    Console.WriteLine("Ids: " + string.Join(", ", ids));
}

static void ViewNotification(NotificationRepository repo)
{
    var id = ReadInt("Enter notification Id: ", min: 1, max: int.MaxValue);
    var n = repo.GetById(id);
    if (n is null)
    {
        Console.WriteLine("Notification not found.");
        return;
    }

    PrintNotification(id, n);
}

static void UpdateNotification(NotificationRepository repo)
{
    var id = ReadInt("Enter notification Id to update: ", min: 1, max: int.MaxValue);
    var n = repo.GetById(id);
    if (n is null)
    {
        Console.WriteLine("Notification not found.");
        return;
    }

    Console.WriteLine("Press ENTER to keep old value.");
    Console.WriteLine($"Current Message: {n.Message}");
    var msg = ReadOptional("New Message: ");
    if (!string.IsNullOrWhiteSpace(msg)) n.Message = msg.Trim();

    repo.Update(id, n);
    Console.WriteLine("Notification updated.");
    PrintNotification(id, n);
}

static void DeleteNotification(NotificationRepository repo)
{
    var id = ReadInt("Enter notification Id to delete: ", min: 1, max: int.MaxValue);
    var deleted = repo.Delete(id);
    if (deleted is null)
    {
        Console.WriteLine("Notification not found.");
        return;
    }

    Console.WriteLine("Notification deleted:");
    PrintNotification(id, deleted);
}

static void PrintUser(User u)
{
    Console.WriteLine($"Name: {u.Name}");
    Console.WriteLine($"Email: {u.Email}");
    Console.WriteLine($"Phone: {u.PhoneNumber}");
}

static void PrintNotification(int id, Notification n)
{
    Console.WriteLine($"Id: {id}");
    Console.WriteLine($"Message: {n.Message}");
    Console.WriteLine($"SentDate: {(n.SentDate == default ? "(not sent yet)" : n.SentDate)}");
}

static string ReadNonEmpty(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var value = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(value))
            return value.Trim();

        Console.WriteLine("Value cannot be empty. Try again.");
    }
}

static string? ReadOptional(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine();
}

static int ReadInt(string prompt, int min, int max)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var value) && value >= min && value <= max)
            return value;

        Console.WriteLine($"Enter a number between {min} and {max}.");
    }
}
