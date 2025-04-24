import MenuIcon from "../../../shared/component-icons/MenuIcon.tsx";
import AllChatsIcon from "../../../shared/component-icons/AllChatsIcon.tsx";
import PrivateChatIcon from "../../../shared/component-icons/PrivateChatIcon.tsx";
import GroupChatIcon from "../../../shared/component-icons/GroupChatIcon.tsx";
import styles from "./Sidebar.module.css"
import SidebarItem from "./SidebarItem/SidebarItem.tsx";
import useChatStore from "../../../store/chat-store/chat-store.ts";
import {ChatType} from "../../../models/chat/ChatType.ts";
import {useState} from "react";


const Sidebar = () => {
    const setTypeFilter = useChatStore(s => s.setUserChatTypeFilter);

    const [activeItem, setActiveItem] = useState<number>(0);

    const items = [
        {
            id: 0,
            icon: AllChatsIcon,
            label: 'All chats',
            action: ()=> setTypeFilter(undefined)
        },
        {
            id: 1,
            icon: GroupChatIcon,
            label: 'Groups',
            action: () => setTypeFilter(ChatType.Group)
        },
        {
            id: 2,
            icon: PrivateChatIcon,
            label: 'Personal',
            action: () => setTypeFilter(ChatType.Private)
        }
    ];


    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <div>
                    <MenuIcon className={styles.menuIcon}/>
                </div>
            </div>

            <div className={styles.content}>
                {items.map(item => (
                    <SidebarItem
                        key={item.id}
                        isActive={activeItem === item.id}
                        icon={item.icon}
                        label={item.label}
                        onClick={() => {
                            item.action();
                            setActiveItem(item.id);
                        }}
                    />
                ))}
            </div>

        </div>
    );
};

export default Sidebar;