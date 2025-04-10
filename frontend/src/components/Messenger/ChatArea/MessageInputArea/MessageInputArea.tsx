import styles from './MessageInputArea.module.css';
import {ChangeEvent, useRef, useState} from "react";
import SendMessageIcon from "../../../../shared/component-icons/SendMessageIcon.tsx";
import {MessageService} from "../../../../api/message-service/MessageService.ts";
import useChatStore from "../../../../store/chat-store.ts";

const MessageInputArea = () => {
    const [text, setText] = useState('');
    const textareaRef = useRef<HTMLTextAreaElement>(null);
    const activeChat = useChatStore(state => state.activeChat);

    const handleChange = (e: ChangeEvent<HTMLTextAreaElement>) => {
        setText(e.target.value);
        if (textareaRef.current) {
            textareaRef.current.style.height = 'auto';
            textareaRef.current.style.height = `${textareaRef.current.scrollHeight}px`;
        }
    };

    const handleKeyDown = (event: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            sendMessage();
        }
    };

    const sendMessage = async (): Promise<void> => {
        if (text.trim() && activeChat) {
            setText('');
            if (textareaRef.current) {
                textareaRef.current.style.height = 'auto';
            }
            debugger
            await MessageService.CreateMessage({

                text,
                chatId: activeChat.id
            })
        }
    }

    return (
        <div className={styles.messageInputArea}>
            <textarea
                ref={textareaRef}
                className={styles.textArea}
                value={text}
                onChange={handleChange}
                onKeyDown={handleKeyDown}
                placeholder="Напишите сообщение..."
            />
            <button className={styles.sendButton} onClick={() => sendMessage()}>
                <SendMessageIcon className={styles.sendIcon}/>
            </button>
        </div>
    );
};

export default MessageInputArea;