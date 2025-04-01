export const isToday = (date: Date): boolean => {
    const today = new Date();

    return (
        date.getDate() === today.getDate() &&
        date.getMonth() === today.getMonth() &&
        date.getFullYear() === today.getFullYear()
    );
}

export const getTimeOnly = (date: Date): string =>{
    const min = String(date.getMinutes()).padStart(2, '0');
    const hour = String(date.getHours()).padStart(2, '0');

    return `${hour}:${min}`;
}


export const getDateOnly = (date:Date):string =>{
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}.${month}.${year}`;
}