﻿namespace BookManagementSystemAuthorizationSystem.RabbitMq.Contracts;

public interface IRabbitMqService
{
    void SendMessage(object obj);
    void SendMessage(string message);
}
