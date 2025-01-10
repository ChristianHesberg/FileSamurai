import {fileTypeFromBuffer} from "file-type";

export class DownloadFileUseCase {
    async execute(file: Buffer, fileName: string): Promise<void> {
        enum AllowedFileTypes {
            JPEG = 'image/jpeg',
            PNG = 'image/png',
            PDF = 'application/pdf',
            DOCX = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
            MP3 = 'audio/mpeg',
            M4A = 'audio/mp4',
            WEBP = 'image/webp'
        }
        try {
            const fileType = await fileTypeFromBuffer(file);
            if(fileType == null) throw new Error("Invalid file type");
            switch (fileType.mime) {
                case AllowedFileTypes.JPEG:
                case AllowedFileTypes.PNG:
                case AllowedFileTypes.WEBP:
                case AllowedFileTypes.PDF:
                case AllowedFileTypes.DOCX:
                case AllowedFileTypes.MP3:
                case AllowedFileTypes.M4A:
                    break;
                default:
                    throw new Error(`File type not allowed: ${fileType.mime}`);
            }
            const blob = new Blob([file], { type: fileType.mime });

            const url = URL.createObjectURL(blob);

            const a = document.createElement('a');
            a.href = url;
            a.download = fileName;

            document.body.appendChild(a);
            a.click();

            URL.revokeObjectURL(url);
            document.body.removeChild(a);

            console.log('File downloaded successfully.');
        } catch (error) {
            console.error(error);
        }
    }
}