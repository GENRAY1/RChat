namespace RChat.Infrastructure.Exceptions;

public class ConfigurationException(string section) 
    : Exception(section + "configuration section is missing or invalid");