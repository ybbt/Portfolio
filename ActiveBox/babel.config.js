module.exports = {
    presets: [
        ['@babel/env', {
            targets: {
                firefox: '60',
                chrome: '67',
                safari: '11.1',
                edge: "17",
				ie: "10",
            },
        }],
    ],
};