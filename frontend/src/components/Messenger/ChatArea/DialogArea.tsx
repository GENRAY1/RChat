import styles from "./DialogArea.module.css"
import MessageItem from "./MessageItem/MessageItem.tsx";
import MessageInputArea from "./MessageInputArea/MessageInputArea.tsx";
import useChatStore from "../../../store/chat-store/chat-store.ts";
import {FC, useCallback, useContext, useEffect, useRef} from "react";
import AuthContext, {AuthContextValue} from "../../../features/auth/AuthContext.ts";
import {ChatType} from "../../../models/chat/ChatType.ts";
import DialogButton from "./DialogButton/DialogButton.tsx";
import {MessageService} from "../../../api/message-service/MessageService.ts";

const DialogArea: FC = () => {
    const messages = useChatStore((store) => store.messages);
    const dialogData = useChatStore((store) => store.dialogData);
    const fetchMessages = useChatStore((store) => store.fetchMessages);
    const lastAppendMessageId = useChatStore((store) => store.lastAppendMessageId);
    const subscribe = useChatStore(s => s.subscribe);
    const createChat = useChatStore(s=> s.createChat);
    const authContext = useContext<AuthContextValue>(AuthContext)


    const messageListRef = useRef<HTMLDivElement>(null);
    const topAnchorRef = useRef<HTMLDivElement>(null);
    const isAtBottomRef = useRef<boolean>(true);


    useEffect(() => {
        const chatId = dialogData?.currentChat?.id
        const anchor = topAnchorRef.current;
        const container = messageListRef.current;
        if (!anchor || !container || !chatId ) return;

        const handleScroll = () => {
            const threshold = 50; // px
            const scrollBottom = container.scrollHeight - container.scrollTop - container.clientHeight;
            isAtBottomRef.current = scrollBottom < threshold;
        };

        container.addEventListener('scroll', handleScroll);

        const observer = new IntersectionObserver(
            async ([entry]) => {
                if (entry.isIntersecting) {
                    const {hasMoreMessages, isLoadingMessages, skipMessages} = useChatStore.getState();

                    if (hasMoreMessages && !isLoadingMessages) {
                        const prevScrollHeight = container.scrollHeight;

                        await fetchMessages(chatId, skipMessages);

                        requestAnimationFrame(() => {
                            const newScrollHeight = container.scrollHeight;

                            container.scrollTop = newScrollHeight - prevScrollHeight;
                        });
                    }
                }
            },
            {
                root: container,
                threshold: 1.0,
            }
        );

        observer.observe(anchor);

        return () => {
            observer.disconnect();
            container.removeEventListener('scroll', handleScroll);
            console.log("observer disconnect ")
        };
    }, [fetchMessages, dialogData?.currentChat?.id]);


    useEffect(() => {
        const container = messageListRef.current;
        if (!container) return;

        if (isAtBottomRef.current) {
            requestAnimationFrame(() => {
                container.scrollTop = container.scrollHeight;
            });
        }
    }, [lastAppendMessageId]);

    const sendMessage = useCallback(async (text:string)=>{
        if(!dialogData)
            throw Error("Failed to retrieve dialog information")

        let chatId = dialogData.currentChat?.id;

        if (!chatId && dialogData.userId && dialogData.needCreatePrivateChat){
            const chat = await createChat({type: ChatType.Private, recipientId: dialogData.userId});
            chatId = chat.id;
        }

        if(!chatId)
            throw Error("Failed to retrieve chat information")

        await MessageService.CreateMessage({text, chatId})
        
    }, [dialogData, createChat]);


    if (!dialogData) {
        return (
            <div className={styles.informBox}>
                <div className={styles.informText}>Select a chat to start messaging</div>
            </div>
        );
    }

    console.log(dialogData?.currentChat?.memberCount)

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <div className={styles.chatName}>{dialogData.displayName}</div>
                <div className={styles.onlineInfo}>
                    {dialogData.currentChat?.type === ChatType.Group
                        ? `${dialogData.currentChat.memberCount} members`
                        : dialogData.currentChat?.type ?? "-"}
                </div>
            </div>
            <div className={styles.messageList} ref={messageListRef}>
                <div ref={topAnchorRef}/>
                {messages.map((m) => (
                    <MessageItem key={m.id} message={m} isOutgoing={m.sender.userId === authContext?.data.user?.id}/>
                ))}
            </div>
            <div className={styles.bottom}>
                {dialogData.currentChat?.type === ChatType.Group && !dialogData.currentChat.hasSubscription
                    ? <DialogButton text={"join group"} onClick={() => subscribe(dialogData.currentChat!.id)}/>
                    : <MessageInputArea sendMessage={sendMessage}/>
                }
            </div>
        </div>
    );
};

export default DialogArea;