const PROXY_CONFIG = [
  {
    context: ["/api"],
    target: "https://localhost:7039",
    secure: false,
    changeOrigin: true, // Ensures requests are as if they are coming from the target
    logLevel: "debug" // For debugging proxy issues
  }
];

module.exports = PROXY_CONFIG;
