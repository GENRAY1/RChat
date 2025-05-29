import {FC, useContext, useEffect, useMemo} from 'react';
import UserChatItem from "./UserChatItem/UserChatItem.tsx";
import useChatStore from "../../../../store/chat-store/chat-store.ts";
import AuthContext, {AuthContextValue} from "../../../../features/auth/AuthContext.ts";


const UserChatList : FC = () => {
    const fetchUserChats = useChatStore(store => store.fetchUserChats)
    const errorMessage  = useChatStore(store => store.fetchUserChatsErrorMsg)
    const activeChatId = useChatStore(store => store.dialogData?.currentChat?.id);
    const selectDialog = useChatStore(store => store.selectDialogFromUserChats)
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

    if(errorMessage)
        return <div>{errorMessage}</div>

    return (
        <div>
            {filteredChats.map((chat) =>
                <UserChatItem
                    key={chat.id}
                    chat={chat}
                    authUserId={data.user!.id}
                    isSelected={chat.id === activeChatId}
                    selectDialog={selectDialog}
                />)
            }
        </div>
    );
};

export default UserChatList;