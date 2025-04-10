import {ChatType} from "./ChatType.ts";
import {ChatGroup} from "./ChatGroup.ts";

export interface Chat {
    id: number;
    type: ChatType;
    creatorId: number;
    createdAt: Date;
    deletedAt?: Date;
    groupChat?: ChatGroup;
}