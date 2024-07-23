function saveFile(filename, bytes) {
    var link = document.createElement("a");
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytes;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}