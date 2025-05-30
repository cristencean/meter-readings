module.exports = {
    transform: {
      '^.+\\.(js|jsx|ts|tsx)$': 'babel-jest',
    },
    transformIgnorePatterns: [
      '/node_modules/(?!(axios)/)',
    ],
    testEnvironment: 'jsdom',
    moduleFileExtensions: ['js', 'jsx', 'ts', 'tsx'],
  };