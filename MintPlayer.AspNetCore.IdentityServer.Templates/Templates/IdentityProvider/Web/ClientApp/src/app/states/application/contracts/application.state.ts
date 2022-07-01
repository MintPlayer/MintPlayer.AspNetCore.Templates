import { User } from "../../../entities/user";

export interface ApplicationState {
    user: User | null;
}