import {SVGProps, FC, MouseEvent, ComponentType} from 'react';
import styles from "../SidebarItem/SidebarItem.module.css";

interface SidebarItemProps {
    icon: ComponentType<SVGProps<SVGSVGElement>>
    label: string;
    onClick?: (event: MouseEvent<HTMLButtonElement>) => void;
}

const SidebarItem:FC<SidebarItemProps> = ({icon:Icon, label, onClick}) => {
    return (
        <button onClick={onClick} className={styles.sidebarItem}>
            <Icon className={styles.sidebarIcon}/>
            <div className={styles.sidebarLabel}>{label}</div>
        </button>
    );
};

export default SidebarItem;