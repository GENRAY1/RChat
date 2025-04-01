import {Message} from "../../models/message/Message.ts";

export const CHAT_MESSAGES: Message[] = [
    {
        id: 2001,
        text: "Доброе утро всем! Готовы к рабочему дню?",
        chatId: 2,
        sender: {
            id: 103,
            firstName: "Мария",
            lastName: "Иванова"
        },
        createdAt: new Date('2023-05-15T08:00:00')
    },
    {
        id: 2002,
        text: "Доброе утро! У меня сегодня презентация в 11:00",
        chatId: 2,
        sender: {
            id: 201,
            firstName: "Алексей"
        },
        createdAt: new Date('2023-05-15T08:05:00'),
        replyToMessageId: 2001
    },
    {
        id: 2003,
        text: "Не забудьте подготовить отчеты до конца дня",
        chatId: 2,
        sender: {
            id: 102,
            firstName: "Игорь",
            lastName: "Петров"
        },
        createdAt: new Date('2023-05-15T09:30:00')
    },
    {
        id: 2004,
        text: "Я уже отправил свой отчет на проверку",
        chatId: 2,
        sender: {
            id: 202,
            firstName: "Елена"
        },
        createdAt: new Date('2023-05-15T10:15:00'),
        updatedAt: new Date('2023-05-15T10:20:00')
    },
    {
        id: 2005,
        text: "Кто-нибудь знает, где документы по проекту Alpha?",
        chatId: 2,
        sender: {
            id: 203,
            firstName: "Дмитрий",
            lastName: "Смирнов"
        },
        createdAt: new Date('2023-05-15T11:45:00')
    },
    {
        id: 2006,
        text: "Они в общей папке на Google Drive",
        chatId: 2,
        sender: {
            id: 103,
            firstName: "Мария",
            lastName: "Иванова"
        },
        createdAt: new Date('2023-05-15T11:47:00'),
        replyToMessageId: 2005
    },
    {
        id: 2007,
        text: "Сегодня в 15:00 созвон по новому проекту",
        chatId: 2,
        sender: {
            id: 102,
            firstName: "Игорь",
            lastName: "Петров"
        },
        createdAt: new Date('2023-05-15T12:30:00')
    },
    {
        id: 2008,
        text: "Я не смогу присутствовать, буду на встрече с клиентом",
        chatId: 2,
        sender: {
            id: 204,
            firstName: "Ольга"
        },
        createdAt: new Date('2023-05-15T12:45:00'),
        replyToMessageId: 2007
    },
    {
        id: 2009,
        text: "Пришлите мне потом запись, пожалуйста",
        chatId: 2,
        sender: {
            id: 204,
            firstName: "Ольга"
        },
        createdAt: new Date('2023-05-15T12:46:00')
    },
    {
        id: 2010,
        text: "Кто-нибудь хочет кофе? Я иду в кафе",
        chatId: 2,
        sender: {
            id: 201,
            firstName: "Алексей"
        },
        createdAt: new Date('2023-05-15T13:15:00')
    },
    {
        id: 2011,
        text: "Мне капучино, пожалуйста!",
        chatId: 2,
        sender: {
            id: 202,
            firstName: "Елена"
        },
        createdAt: new Date('2023-05-15T13:16:00'),
        replyToMessageId: 2010
    },
    {
        id: 2012,
        text: "Мне тоже, спасибо!",
        chatId: 2,
        sender: {
            id: 203,
            firstName: "Дмитрий",
            lastName: "Смирнов"
        },
        createdAt: new Date('2023-05-15T13:17:00'),
        replyToMessageId: 2010
    },
    {
        id: 2013,
        text: "Только что отправил всем ссылку на созвон",
        chatId: 2,
        sender: {
            id: 102,
            firstName: "Игорь",
            lastName: "Петров"
        },
        createdAt: new Date('2023-05-15T14:55:00')
    },
    {
        id: 2014,
        text: "У меня проблемы с интернетом, могу опоздать на пару минут",
        chatId: 2,
        sender: {
            id: 205,
            firstName: "Артем"
        },
        createdAt: new Date('2023-05-15T14:59:00')
    },
    {
        id: 2015,
        text: "Встреча переносится на 15:30, ждем всех",
        chatId: 2,
        sender: {
            id: 102,
            firstName: "Игорь",
            lastName: "Петров"
        },
        createdAt: new Date('2023-05-15T15:05:00'),
        updatedAt: new Date('2023-05-15T15:10:00')
    },
    {
        id: 2016,
        text: "Вот кофе, как и обещал ☕",
        chatId: 2,
        sender: {
            id: 201,
            firstName: "Алексей"
        },
        createdAt: new Date('2023-05-15T15:20:00'),
        replyToMessageId: 2010
    },
    {
        id: 2017,
        text: "Спасибо! Как раз вовремя перед созвоном",
        chatId: 2,
        sender: {
            id: 202,
            firstName: "Елена"
        },
        createdAt: new Date('2023-05-15T15:21:00'),
        replyToMessageId: 2016
    },
    {
        id: 2018,
        text: "Только что удалил старое сообщение с неправильным временем",
        chatId: 2,
        sender: {
            id: 102,
            firstName: "Игорь",
            lastName: "Петров"
        },
        createdAt: new Date('2023-05-15T15:15:00'),
        deletedAt: new Date('2023-05-15T15:16:00')
    },
    {
        id: 2019,
        text: "Все, я подключился к созвону",
        chatId: 2,
        sender: {
            id: 205,
            firstName: "Артем"
        },
        createdAt: new Date('2023-05-15T15:28:00')
    },
    {
        id: 2020,
        text: "Отличная работа сегодня, всем спасибо! Завтра продолжаем",
        chatId: 2,
        sender: {
            id: 102,
            firstName: "Игорь",
            lastName: "Петров"
        },
        createdAt: new Date('2023-05-15T18:00:00')
    }
];