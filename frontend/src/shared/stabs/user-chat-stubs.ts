import {UserChat} from "../../models/chat/UserChat.ts";
import {ChatType} from "../../models/chat/ChatType.ts";

export const USER_CHATS:UserChat[] = [
    {
        id: 1,
        displayName: "Алексей Петров",
        type: ChatType.Private,
        creatorId: 101,
        createdAt: new Date('2023-05-10'),
        groupChat: {
            name: "",
            description: "",
            isPrivate: true
        },
        lastMessage: {
            id: 1001,
            text: "Привет, как дела?",
            chatId: 1,
            sender: {
                id: 101,
                firstName: "Алексей",
                lastName: "Петров"
            },
            createdAt: new Date()
        }
    },
    {
        id: 2,
        displayName: "Рабочая группа",
        type: ChatType.Group,
        creatorId: 102,
        createdAt: new Date('2023-05-12'),
        groupChat: {
            name: "Рабочая группа",
            description: "Обсуждение проектов",
            isPrivate: false
        },
        lastMessage: {
            id: 1002,
            text: "Завтра встреча в 10:00",
            chatId: 2,
            sender: {
                id: 103,
                firstName: "Мария",
                lastName: "Иванова"
            },
            createdAt: new Date('2023-05-15T09:15:00')
        }
    },
    {
        id: 3,
        displayName: "Друзья",
        type: ChatType.Group,
        creatorId: 104,
        createdAt: new Date('2023-05-15'),
        groupChat: {
            name: "Друзья",
            description: "Чат для общения",
            isPrivate: true
        },
        lastMessage: {
            id: 1003,
            text: "Кто сегодня свободен?",
            chatId: 3,
            sender: {
                id: 105,
                firstName: "Дмитрий"
            },
            createdAt: new Date('2023-05-16T18:20:00')
        }
    },
    {
        id: 4,
        displayName: "Елена Смирнова",
        type: ChatType.Private,
        creatorId: 106,
        createdAt: new Date('2023-05-18'),
        groupChat: {
            name: "",
            description: "",
            isPrivate: true
        },
        lastMessage: {
            id: 1004,
            text: "Спасибо за помощь!",
            chatId: 4,
            sender: {
                id: 106,
                firstName: "Елена",
                lastName: "Смирнова"
            },
            createdAt: new Date('2023-05-20T12:45:00')
        }
    },
    {
        id: 5,
        displayName: "Фитнес клуб",
        type: ChatType.Group,
        creatorId: 107,
        createdAt: new Date('2023-05-20'),
        groupChat: {
            name: "Фитнес клуб",
            description: "Чат участников",
            isPrivate: false
        },
        lastMessage: {
            id: 1005,
            text: "Завтра тренировка в 8 утра",
            chatId: 5,
            sender: {
                id: 108,
                firstName: "Антон"
            },
            createdAt: new Date('2023-05-22T07:30:00'),
            updatedAt: new Date('2023-05-22T07:35:00')
        }
    },
    {
        id: 6,
        displayName: "Иван Козлов",
        type: ChatType.Private,
        creatorId: 109,
        createdAt: new Date('2023-05-22'),
        groupChat: {
            name: "",
            description: "",
            isPrivate: true
        },
        lastMessage: {
            id: 1006,
            text: "Где встретимся?",
            chatId: 6,
            sender: {
                id: 109,
                firstName: "Иван",
                lastName: "Козлов"
            },
            createdAt: new Date('2023-05-23T16:10:00')
        }
    },
    {
        id: 7,
        displayName: "Путешествия",
        type: ChatType.Group,
        creatorId: 110,
        createdAt: new Date('2023-05-25'),
        groupChat: {
            name: "Путешествия",
            description: "Планирование поездок",
            isPrivate: true
        },
        lastMessage: {
            id: 1007,
            text: "Билеты куплены!",
            chatId: 7,
            sender: {
                id: 111,
                firstName: "Ольга",
                lastName: "Новикова"
            },
            createdAt: new Date('2023-05-26T20:00:00')
        }
    },
    {
        id: 8,
        displayName: "Сергей Волков",
        type: ChatType.Private,
        creatorId: 112,
        createdAt: new Date('2023-05-28'),
        groupChat: {
            name: "",
            description: "",
            isPrivate: true
        },
        lastMessage: {
            id: 1008,
            text: "Документы готовы",
            chatId: 8,
            sender: {
                id: 112,
                firstName: "Сергей",
                lastName: "Волков"
            },
            createdAt: new Date('2023-05-29T11:20:00'),
            deletedAt: new Date('2023-05-29T11:25:00')
        }
    },
    {
        id: 9,
        displayName: "Кино клуб",
        type: ChatType.Group,
        creatorId: 113,
        createdAt: new Date('2023-06-01'),
        groupChat: {
            name: "Кино клуб",
            description: "Обсуждение фильмов",
            isPrivate: false
        },
        lastMessage: {
            id: 1009,
            text: "Сегодня смотрим 'Начало'",
            chatId: 9,
            sender: {
                id: 114,
                firstName: "Александр"
            },
            createdAt: new Date('2023-06-02T19:00:00')
        }
    },
    {
        id: 10,
        displayName: "Анна Кузнецова",
        type: ChatType.Private,
        creatorId: 115,
        createdAt: new Date('2023-06-05'),
        groupChat: {
            name: "",
            description: "",
            isPrivate: true
        },
        lastMessage: {
            id: 1010,
            text: "Как твои дела?",
            chatId: 10,
            sender: {
                id: 115,
                firstName: "Анна",
                lastName: "Кузнецова"
            },
            createdAt: new Date('2023-06-06T10:15:00')
        }
    },
    {
        id: 11,
        displayName: "IT отдел",
        type: ChatType.Group,
        creatorId: 116,
        createdAt: new Date('2023-06-10'),
        groupChat: {
            name: "IT отдел",
            description: "Технические вопросы",
            isPrivate: false
        },
        lastMessage: {
            id: 1011,
            text: "Сервер будет обновлен завтра",
            chatId: 11,
            sender: {
                id: 117,
                firstName: "Павел",
                lastName: "Соколов"
            },
            createdAt: new Date('2023-06-11T15:45:00'),
            updatedAt: new Date('2023-06-11T15:50:00')
        }
    },
    {
        id: 12,
        displayName: "Дмитрий Федоров",
        type: ChatType.Private,
        creatorId: 118,
        createdAt: new Date('2023-06-15'),
        groupChat: {
            name: "",
            description: "",
            isPrivate: true
        },
        lastMessage: {
            id: 1012,
            text: "Договор подписан",
            chatId: 12,
            sender: {
                id: 118,
                firstName: "Дмитрий",
                lastName: "Федоров"
            },
            createdAt: new Date('2023-06-16T12:30:00')
        }
    },
    {
        id: 13,
        displayName: "Книжный клуб",
        type: ChatType.Group,
        creatorId: 119,
        createdAt: new Date('2023-06-20'),
        groupChat: {
            name: "Книжный клуб",
            description: "Обсуждение книг",
            isPrivate: true
        },
        lastMessage: {
            id: 1013,
            text: "Следующая книга - '1984'",
            chatId: 13,
            sender: {
                id: 120,
                firstName: "Екатерина",
                lastName: "Морозова"
            },
            createdAt: new Date('2023-06-22T17:00:00')
        }
    },
    {
        id: 14,
        displayName: "Ольга Белова",
        type: ChatType.Private,
        creatorId: 121,
        createdAt: new Date('2023-06-25'),
        groupChat: {
            name: "",
            description: "",
            isPrivate: true
        },
        lastMessage: {
            id: 1014,
            text: "Фото получила, спасибо!",
            chatId: 14,
            sender: {
                id: 121,
                firstName: "Ольга",
                lastName: "Белова"
            },
            createdAt: new Date('2023-06-26T09:45:00'),
            deletedAt: new Date('2023-06-26T09:50:00')
        }
    },
    {
        id: 15,
        displayName: "Футбол",
        type: ChatType.Group,
        creatorId: 122,
        createdAt: new Date('2023-06-30'),
        groupChat: {
            name: "Футбол",
            description: "Организация матчей",
            isPrivate: false
        },
        lastMessage: {
            id: 1015,
            text: "Завтра игра в 19:00",
            chatId: 15,
            sender: {
                id: 123,
                firstName: "Артем"
            },
            createdAt: new Date('2023-07-01T18:30:00')
        }
    }
];