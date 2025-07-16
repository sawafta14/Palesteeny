document.addEventListener('DOMContentLoaded', () => {
    const input = document.querySelector('.input-wrapper input');
    const resultsBox = document.querySelector('.search-results');

    if (!input || !resultsBox) return;

    input.addEventListener('input', async function () {
        const query = this.value.trim();

        if (!query) {
            resultsBox.style.display = 'none';
            resultsBox.innerHTML = '';
            return;
        }

        try {
            const response = await fetch(`/api/search?term=${encodeURIComponent(query)}`);
            const results = await response.json();

            if (results.length === 0) {
                resultsBox.innerHTML = '<div style="padding: 10px;">لا توجد نتائج</div>';
            } else {
                resultsBox.innerHTML = results.map(group => `
                    ${group.items.map(item => {
                                    const firstWord = item.description?.trim().split(/\s+/)[0] || '';
                                    return `
                            <a href="${item.url}" class="search-item">
                                ${firstWord}
                            </a>
                        `;
                                }).join('')}
                `).join('');


            }

            resultsBox.style.display = 'block';
        } catch (error) {
            console.error('Search error:', error);
        }
    });

    document.addEventListener('click', (e) => {
        if (!e.target.closest('.input-wrapper')) {
            resultsBox.style.display = 'none';
        }
    });
});
