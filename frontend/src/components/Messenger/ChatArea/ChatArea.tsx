import styles from "./ChatArea.module.css"
import MessageItem from "./MessageItem/MessageItem.tsx";
import MessageInputArea from "./MessageInputArea/MessageInputArea.tsx";
import useChatStore from "../../../store/chat-store/chat-store.ts";
import {FC, useContext, useEffect, useRef} from "react";
import AuthContext, {AuthContextValue} from "../../../features/auth/AuthContext.ts";

const ChatArea: FC = () => {
    const messages = useChatStore((store) => store.messages);
    const activeChat = useChatStore((store) => store.activeChat);
    const fetchMessages = useChatStore((store) => store.fetchMessages);
    const lastAppendMessageId = useChatStore((store) => store.lastAppendMessageId);
    const authContext = useContext<AuthContextValue>(AuthContext)


    const messageListRef = useRef<HTMLDivElement>(null);
    const topAnchorRef = useRef<HTMLDivElement>(null);
    const isAtBottomRef = useRef<boolean>(true);


    useEffect(() => {
        const anchor = topAnchorRef.current;
        const container = messageListRef.current;
        if (!anchor || !container) return;

        const handleScroll = () => {
            const threshold = 50; // px
            const scrollBottom = container.scrollHeight - container.scrollTop - container.clientHeight;
            isAtBottomRef.current = scrollBottom < threshold;
        };

        container.addEventListener('scroll', handleScroll);

        const observer = new IntersectionObserver(
            async ([entry]) => {
                if (entry.isIntersecting) {
                    const hasMoreMessages = useChatStore.getState().hasMoreMessages;
                    const isLoadingMessages = useChatStore.getState().isLoadingMessages;


                    if (hasMoreMessages && !isLoadingMessages) {
                        const prevScrollHeight = container.scrollHeight;

                        await fetchMessages();

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
        };
    }, [fetchMessages, activeChat?.id]);


    useEffect(() => {
        const container = messageListRef.current;
        if (!container) return;

        if (isAtBottomRef.current) {
            requestAnimationFrame(() => {
                container.scrollTop = container.scrollHeight;
            });
        }
    }, [lastAppendMessageId]);

    if (!activeChat) {
        return (
            <div className={styles.informBox}>
                <div className={styles.informText}>Select a chat to start messaging</div>
            </div>
        );
    }

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <div className={styles.chatName}>{activeChat.displayName}</div>
                <div className={styles.onlineInfo}>{activeChat.type}</div>
            </div>
            <div className={styles.messageList} ref={messageListRef}>
                <div ref={topAnchorRef}/>
                {messages.map((m) => (
                    <MessageItem key={m.id} message={m} isOutgoing={m.sender.userId === authContext?.data.user?.id}/>
                ))}
            </div>
            <div className={styles.bottom}>
                <MessageInputArea />
            </div>
        </div>
    );
};

export default ChatArea;