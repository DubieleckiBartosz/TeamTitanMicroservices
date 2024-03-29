﻿namespace Shared.Implementations.Communications.Email;

public class EmailOptions
{
    public int Port { get; set; }
    public string FromName { get; set; }
    public string Host { get; set; }
    public string FromAddress { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public bool LocalMail { get; set; }
}