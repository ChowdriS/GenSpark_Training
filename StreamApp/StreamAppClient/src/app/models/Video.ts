export interface Video {
    id: string;
    title: string;
    description: string;
    blobUrl: string;
}

export interface VideoUploadPayload{
    title: string;
    description: string;
    file: File;
}