import {FC} from 'react';
import {getInitials, generateColorFromName, getContrastColor} from "../../utils/name-avatar-utils.ts";

interface NameAvatarProps {
    name: string;
    size: number;
    fontSize: number;
    className?: string;
}

const NameAvatar: FC<NameAvatarProps> = ({name, size, fontSize, className}) => {

    const initials: string = getInitials(name);
    const backgroundColorHash: string = generateColorFromName(name);
    const textColorHash: string = getContrastColor(backgroundColorHash)

    return (
        <div className={className}
            style={{
                width: `${size}px`,
                height: `${size}px`,
                borderRadius: '50%',
                backgroundColor: backgroundColorHash,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                color: textColorHash,
                fontSize: `${fontSize}px`,
                fontWeight: 'bold',
                fontFamily: 'Arial, sans-serif',
            }}
        >
            {initials}
        </div>
    );
};

export default NameAvatar;