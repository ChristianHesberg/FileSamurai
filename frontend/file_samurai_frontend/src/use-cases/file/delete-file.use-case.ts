import {FileService} from "../../services/file.service";

export class DeleteFileUseCase {
    constructor(private readonly fileService: FileService) {
    }

    async execute(fileId: string) {
        return await this.fileService.deleteFile(fileId)
    }
}