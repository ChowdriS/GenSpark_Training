const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveNotification", (message, notificationType) => {
    console.log(`Notification (${notificationType}): ${message}`);
    // Display notification to user using toast/alert
});

connection.start().then(() => {
    // Join user-specific group
    connection.invoke("JoinGroup", `user_${currentUserId}`);
    
    // Join admin group if user is admin
    if (userRole === "Admin") {
        connection.invoke("JoinGroup", "admins");
    }
    
    // Join manager group if user is manager
    if (userRole === "Manager") {
        connection.invoke("JoinGroup", `manager_${currentUserId}`);
    }
});