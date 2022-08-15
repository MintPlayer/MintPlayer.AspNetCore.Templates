import { ELoginStatus } from "../enums/login-status";
import { User } from "./user";

export interface LoginResult {
	status: ELoginStatus;
	user: User | null;
	error: string;
	errorDescription: string;
}
