import MenuIcon from "../../shared/component-icons/MenuIcon.tsx";
import AllChatsIcon from "../../shared/component-icons/AllChatsIcon.tsx";
import PrivateChatIcon from "../../shared/component-icons/PrivateChatIcon.tsx";
import GroupChatIcon from "../../shared/component-icons/GroupChatIcon.tsx";


import styles from "./Sidebar.module.css"
import SidebarItem from "./SidebarItem/SidebarItem.tsx";

const Sidebar = () => {
    return (
        <div className={styles.sidebar}>
            <div className={styles.header}>
                <div>
                    <MenuIcon className={styles.menuIcon}/>
                </div>
            </div>

            <div className={styles.content}>
                <SidebarItem icon={AllChatsIcon} label="All chats"/>
                <SidebarItem icon={GroupChatIcon} label="Groups"/>
                <SidebarItem icon={PrivateChatIcon} label="Personal"/>
            </div>

        </div>
    );
};

export default Sidebar;