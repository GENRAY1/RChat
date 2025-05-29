import styles from './DialogButton.module.css'
import {FC} from "react";


interface DialogButtonProps{
    text: string;
    onClick?: () => void;
}

const DialogButton: FC<DialogButtonProps> = ({text, onClick}) => {
    return (
        <button className={styles.container} onClick={onClick}>
            {text}
        </button>
    );
};

export default DialogButton;