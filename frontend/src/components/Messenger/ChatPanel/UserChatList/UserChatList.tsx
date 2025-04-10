import {useContext, useEffect} from 'react';
import UserChatItem from "../UserChatItem/UserChatItem.tsx";
import styles from "./UserChatList.module.css"
import useChatStore from "../../../../store/chat-store.ts";
import {UserChat} from "../../../../models/chat/UserChat.ts";
import AuthContext, {AuthContextValue} from "../../../../features/auth/AuthContext.ts";

const UserChatList = () => {
    const userChats : UserChat[] = useChatStore(store => store.userChats)
    const fetchUserChats = useChatStore(store => store.fetchChats)
    const errorMessage  = useChatStore(store => store.fetchChatsErrorMsg)
    const activateChat = useChatStore(store => store.activateChat);
    const activeChatId = useChatStore(store => store.activeChat?.id);

    const {data} : AuthContextValue = useContext<AuthContextValue>(AuthContext)

    useEffect(() => {
        fetchUserChats();
    }, [fetchUserChats])

    const userChatElements = userChats.map((chat) =>
        <UserChatItem
            key={chat.id}
            chat={chat}
            isLastMessageOutgoing={chat.latestMessage ? data.user?.id === chat.latestMessage.sender.userId : false}
            isActive={chat.id === activeChatId}
            onActivate={activateChat}
        />);

    return (
        <div className={styles.container}>
            {
                errorMessage
                ? <div>{errorMessage}</div>
                : userChatElements
            }
        </div>
    );
};

export default UserChatList;