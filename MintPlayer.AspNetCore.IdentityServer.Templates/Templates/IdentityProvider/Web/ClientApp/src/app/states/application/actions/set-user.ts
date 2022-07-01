import { User } from "../../../entities/user";
import { Metadata } from "../metadata";

export class SetUser {
    constructor(public user: User | null) {}
    static readonly type = `${Metadata.namespace}.SetUser`;
}