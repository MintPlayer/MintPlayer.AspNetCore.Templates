import { User } from "./user";

export interface RegisterResult {
	requiresEmailConfirmation: boolean;
	user?: User;
}
