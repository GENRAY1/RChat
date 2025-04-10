import {SVGProps, FC, MouseEvent, ComponentType} from 'react';
import styles from "./SidebarItem.module.css";

interface SidebarItemProps {
    icon: ComponentType<SVGProps<SVGSVGElement>>
    label: string;
    onClick?: (event: MouseEvent<HTMLDivElement>) => void;
}

const SidebarItem:FC<SidebarItemProps> = ({icon:Icon, label, onClick}) => {
    return (
        <div onClick={onClick} className={styles.sidebarItem}>
            <Icon className={styles.sidebarIcon}/>
            <div className={styles.sidebarLabel}>{label}</div>
        </div>
    );
};

export default SidebarItem;