const PROXY_CONFIG = [
  {
    context: [
      // Common
      '/api',

      // VisualStudio endpoints for debugging and hot-reload
      '/_framework',
      '/_vs',
    ],
    target: 'http://localhost:5000',
    secure: false,
    ws: true,
  },
];

module.exports = PROXY_CONFIG;
