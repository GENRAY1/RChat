export function getString(key: string): string | undefined {
    const value = localStorage.getItem(key);
    return value !== null ? value : undefined;
}

export function getNumber(key: string): number | undefined {
    const value = localStorage.getItem(key);
    return value !== null ? parseInt(value) : undefined;
}

export function getParsed<T>(key: string): T | undefined {
    const value = localStorage.getItem(key);
    return value !== null ? JSON.parse(value) as T : undefined;
}