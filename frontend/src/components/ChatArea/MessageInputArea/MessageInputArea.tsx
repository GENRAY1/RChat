import styles from './MessageInputArea.module.css';
import {ChangeEvent, useRef, useState} from "react";
import SendMessageIcon from "../../../shared/component-icons/SendMessageIcon.tsx";

const MessageInputArea = () => {
    const [text, setText] = useState('');
    const textareaRef = useRef<HTMLTextAreaElement>(null);

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

    const sendMessage = (): void => {
        if (text.trim()) {
            setText('');
            if (textareaRef.current) {
                textareaRef.current.style.height = 'auto';
            }
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