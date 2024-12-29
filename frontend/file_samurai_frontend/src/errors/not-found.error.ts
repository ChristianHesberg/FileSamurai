import {HttpError} from "./http.error";

export class NotFoundError extends HttpError {
    constructor(data: unknown, message = 'Resource not found') {
        super(message, 404, data);
    }
}