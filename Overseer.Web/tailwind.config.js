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
        'light-background': '#F4F3F2',
        'dark-background': '#121212',
        'light-surface': '#fafafa',
        'dark-surface': '#1E1E1E',
        'light-foreground': '#fefefe',
        'dark-foreground': '#2E2E2E',
        'primary': '#FF359A',
        'accent': '#B035FF',
      },
    },
  }
}