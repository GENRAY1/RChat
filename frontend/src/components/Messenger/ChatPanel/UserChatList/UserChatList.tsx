import {useContext, useEffect, useMemo} from 'react';
import UserChatItem from "../UserChatItem/UserChatItem.tsx";
import styles from "./UserChatList.module.css"
import useChatStore from "../../../../store/chat-store.ts";
import AuthContext, {AuthContextValue} from "../../../../features/auth/AuthContext.ts";

const UserChatList = () => {
    const fetchUserChats = useChatStore(store => store.fetchUserChats)
    const errorMessage  = useChatStore(store => store.fetchUserChatsErrorMsg)
    const activateChat = useChatStore(store => store.activateChat);
    const activeChatId = useChatStore(store => store.activeChat?.id);
    const userChats = useChatStore(state => state.userChats);
    const userChatFilter = useChatStore(state => state.userChatTypeFilter);
    const {data} : AuthContextValue = useContext<AuthContextValue>(AuthContext)

    useEffect(() => {
        fetchUserChats();
    }, [fetchUserChats])

    const filteredChats = useMemo(()=>{
        if(!userChatFilter) return userChats;

        return userChats.filter(chat => chat.type === userChatFilter);
    }, [userChats, userChatFilter])


    const userChatElements = filteredChats.map((chat) =>
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