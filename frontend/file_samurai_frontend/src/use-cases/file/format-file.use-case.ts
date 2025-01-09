import {fileTypeFromBuffer} from "file-type";

export class FormatFileUseCase{

    async execute(file: Buffer): Promise<void> {
        try {
            
            const fileType = await fileTypeFromBuffer(file);
            if(fileType == null) throw new Error("Invalid file type");
            // Create a Blob from the decrypted content
            const blob = new Blob([file], { type: fileType.mime });

            // Create a URL for the Blob
            const url = URL.createObjectURL(blob);

            // Create a link element
            const a = document.createElement('a');
            a.href = url;
            a.download = 'your download link';

            // Append the link to the document body and trigger a click event
            document.body.appendChild(a);
            a.click();

            // Clean up by revoking the Object URL and removing the link element
            URL.revokeObjectURL(url);
            document.body.removeChild(a);

            console.log('File downloaded successfully.');
        } catch (error) {
            console.error(error);
        }
    }
}