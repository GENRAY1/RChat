export const generateColorFromName = (name:string) => {
    if (!name) return '#cccccc';

    let hash = 0;
    for (let i = 0; i < name.length; i++) {
        hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }

    let color = '#';
    for (let i = 0; i < 3; i++) {
        const value = (hash >> (i * 8)) & 0xFF;
        color += ('00' + value.toString(16)).substr(-2);
    }

    return color;
};

export const getInitials = (name:string) => {
    if (!name) return '';

    const names = name.split(' ');
    let initials = names[0].substring(0, 1).toUpperCase();

    if (names.length > 1) {
        initials += names[names.length - 1].substring(0, 1).toUpperCase();
    }

    return initials;
};

export const getContrastColor = (hexColor:string) => {
    const r:number = parseInt(hexColor.slice(1, 3), 16);
    const g:number = parseInt(hexColor.slice(3, 5), 16);
    const b:number = parseInt(hexColor.slice(5, 7), 16);

    const brightness = (r * 299 + g * 587 + b * 114) / 1000;
    return brightness > 128 ? '#000000' : '#FFFFFF';
};