import {AccountRole} from "../../models/account/AccountRole.ts";
import {User} from "../../models/user/User.ts";

export interface AuthData {
    accessToken?: string,
    accountId?: number,
    accountRole?: AccountRole,
    user?: User,
}