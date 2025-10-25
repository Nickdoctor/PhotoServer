import { useState } from 'react';
import { useEffect } from 'react';
export default function App() {
    const [selectedFile, setSelectedFile] = useState(null);
    const [uploadStatus, setUploadStatus] = useState('');
    const [serverConnected, setServerConnected] = useState(false);

    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    const handleUpload = async () => {
        if (!selectedFile) {
            setUploadStatus('Please select a file first.');
            return;
        }

        const formData = new FormData();
        formData.append('file', selectedFile);

        try {
            const response = await fetch('http://localhost:5081/api/Upload', {
                method: 'POST',
                body: formData,
            });
            if (response.ok) {
                const data = await response.json();
                console.log("Upload response:", data);
                setUploadStatus('Upload successful!');
            } else {
                const data = await response.json();
                console.log("Upload response:", data);
                setUploadStatus('Upload failed.');
            }
        } catch (error) {
            console.error(error);
            setUploadStatus('An error occurred during upload.');
        }
    };

    useEffect(() => { // Cleanup old uploads on component mount or first render
        fetch("http://localhost:5081/api/upload/cleanup", { method: "POST" })
            .then(async res => {
                try {
                    const data = await res.json();
                    console.log(data.message);
                    setServerConnected(true);
                } catch {
                    console.log("Cleanup response not JSON", await res.text());
                }
            })
            .catch(err => console.error(err));
    }, []);

    return (
        <div className="flex flex-col items-center justify-center h-screen border-4 border-gray-300 rounded-lg p-8">
            <h1 className="text-green-600 font-bold">{serverConnected ? "Server Connected!" : "Connecting to Server..."}</h1>
            <p className="mb-4 text-gray-600">Select image file(s) to upload to the server.</p>
            <div className="flex flex-col items-center justify-center w-full max-w-sm">
                <label className="inline-block">
                    <input
                        type="file"
                        onChange={handleFileChange}
                        className="
        block w-full text-sm text-gray-500
        file:mr-4 file:py-2 file:px-4
        file:rounded-full file:border-0
        file:text-sm file:font-semibold
        file:bg-blue-100 file:text-blue-700
        hover:file:bg-blue-200
        cursor-pointer
      "
                    />
                </label>
            </div>
            <button onClick={handleUpload} className="bg-blue-500 text-white py-2 px-4 rounded my-5">
                Upload
            </button>
            <p className="mt-4">{uploadStatus}</p>
        </div>
    );
}