import {FileService} from "../../services/file.service";

export class DeleteUserFileAccessUseCase {
    constructor(private readonly fileService: FileService) {
    }

    async execute(userId: string, fileId: string) {
        return await this.fileService.deleteUserFileAccess(userId, fileId)
    }
}
