const express = require('express');

const app = express();
const PORT = process.env.PORT || 3000;

app.use(express.json());

app.get('/', (req, res) => {
    res.json({
        service: "infra-mapper-backend",
        status: "running",
        port: PORT
    });
});

app.get('/health', (req, res) => {
    res.send("OK");
});

app.listen(PORT, () => {
    console.log(`Backend listening on port ${PORT}`);
});
