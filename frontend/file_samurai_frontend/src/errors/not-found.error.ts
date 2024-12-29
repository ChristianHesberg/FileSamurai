import {HttpError} from "./http.error";

export class NotFoundError extends HttpError {
    constructor(data?: unknown | undefined, message = 'Resource not found') {
        super(message, 404, data);
    }
}