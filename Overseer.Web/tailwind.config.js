/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './**/*.html',
    './**/*.cshtml',
    './**/*.razor',
    './**/*.cs',
    './*.razor'
  ],
  darkMode: 'class',
  theme: {
    fontFamily: {
      'body': ['"JetBrains Mono"', 'Roboto', 'monospace'],
    },
    extend: {
      colors: {
        'background': '#121212',
        'surface': '#171717',
        'foreground': '#212121',
        'highlight': '#313131',
      },
    },
  }
}