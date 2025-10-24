import { useState } from 'react';
import { useEffect } from 'react';
export default function App() {
    const [selectedFile, setSelectedFile] = useState(null);
    const [uploadStatus, setUploadStatus] = useState('');

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
            } catch {
                console.log("Cleanup response not JSON", await res.text());
            }
        })
        .catch(err => console.error(err));
}, []);

    return (
        <div style={{ maxWidth: 400, margin: '80px auto', textAlign: 'center' }}>
            <h1>Upload an Image</h1>
            <input type="file" onChange={handleFileChange} />
            <br /><br />
            <button onClick={handleUpload}>Upload</button>
            <p>{uploadStatus}</p>
        </div>
    );
}
