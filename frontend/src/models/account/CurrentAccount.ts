import {User} from "../user/User.ts";
import {AccountRole} from "./AccountRole.ts";

export interface CurrentAccount {
    id: number,
    user?: User,
    role: AccountRole
}