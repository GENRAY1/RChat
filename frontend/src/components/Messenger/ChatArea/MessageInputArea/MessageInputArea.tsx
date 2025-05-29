import styles from './MessageInputArea.module.css';
import {ChangeEvent, FC, KeyboardEvent, useRef, useState} from "react";
import SendMessageIcon from "../../../../shared/component-icons/SendMessageIcon.tsx";

interface MessageInputAreaProps{
    sendMessage: (message:string) => Promise<void>;
}

const MessageInputArea : FC<MessageInputAreaProps> = ({sendMessage}) => {
    const [text, setText] = useState('');
    const textareaRef = useRef<HTMLTextAreaElement>(null);


    const handleChange = (e: ChangeEvent<HTMLTextAreaElement>) => {
        setText(e.target.value);
        if (textareaRef.current) {
            textareaRef.current.style.height = 'auto';
            textareaRef.current.style.height = `${textareaRef.current.scrollHeight}px`;
        }
    };

    const handleKeyDown = async (event: KeyboardEvent<HTMLTextAreaElement>) => {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            await send();
        }
    };

    const send = async (): Promise<void> => {
        if (text.trim()) {
            setText('');
            if (textareaRef.current) {
                textareaRef.current.style.height = 'auto';
            }
        }

        await sendMessage(text.trim())
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
            <button className={styles.sendButton} onClick={async () => await send()}>
                <SendMessageIcon className={styles.sendIcon}/>
            </button>
        </div>
    );
};

export default MessageInputArea;