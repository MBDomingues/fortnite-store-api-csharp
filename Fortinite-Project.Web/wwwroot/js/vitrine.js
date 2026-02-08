/**
 * Classe principal VitrineJS
 * Adaptada para ASP.NET Core MVC
 */
class VitrineJS {
    constructor(userToken) {
        this.user = userToken || null;
        
        // ALTERAÇÃO 1: Url Relativa para bater no seu Controller local/docker
        this.API_BASE_URL = '/api'; 
        
        this.itens = []; 
        this.userData = null;
        this.todosOsItens = []; 
        this.todosOsItensCarregados = false;
        this.usuarios = [];
        this.usuariosCarregados = false;
        this.historico = [];
        this.historicoCarregado = false;
        this.itensAdquiridosSet = new Set();
        this.carouselIndicators = null;
        this.itemModalElement = null;
        this.currentItemInModal = null;

        this.coresPadraoRaridade = {
            'comum': '#b0b0b0', 'common': '#b0b0b0',
            'incomum': '#60aa3a', 'uncommon': '#60aa3a',
            'raro': '#4ec1f3', 'rare': '#4ec1f3',
            'épico': '#bf6ee0', 'epic': '#bf6ee0',
            'lendário': '#e9a748', 'legendary': '#e9a748'
        };

        this.init();
    }

    // --- MAPA CENTRALIZADO DE TRADUÇÃO DE TIPOS ---
    _getTipoMapa(outputCase = 'lower') {
        const map = {
            'outfit': 'Traje', 'skin': 'Traje', 'traje': 'Traje',
            'backpack': 'Acessório para as Costas', 'mochila': 'Acessório para as Costas',
            'pickaxe': 'Picareta', 'picareta': 'Picareta',
            'glider': 'Asa-delta', 'asa-delta': 'Asa-delta',
            'emote': 'Gesto', 'gesto': 'Gesto',
            'wrap': 'Envelopamento', 'envelopamento': 'Envelopamento',
            'pet': 'Mascote', 'mascote': 'Mascote',
            'shoes': 'Sapatos', 'sapatos': 'Sapatos',
            'contrail': 'Rastro de Fumaça', 'rastro': 'Rastro de Fumaça',
            'banner': 'Estandarte', 'estandarte': 'Estandarte',
            'chassis': 'Chassi', 'chassi': 'Chassi',
            'wheel': 'Roda', 'roda': 'Roda',
            'decal': 'Decalque', 'decalque': 'Decalque',
            'music': 'Pacote de Música', 'musica': 'Pacote de Música',
            'loading': 'Tela de Carregamento'
        };

        if (outputCase === 'api') return map; // Retorna com capitulação correta
        return map;
    }

    async init() {
        // Se houver token, busca dados do usuário (Requer UsersController implementado no C#)
        if (this.user) {
            await this.verificaUsuario();
        }
        
        // Busca Loja
        this.buscaItensDisponiveis();
        
        // Configura elementos da DOM
        this.pegaElementos();
    }

    pegaElementos() {
        this.cosmeticosGrid = document.getElementById('shop-grid'); 
        this.carrouselItems = document.getElementById('carouselItems');
        this.navItens = document.getElementById('nav-itens');
        this.carouselIndicators = document.querySelector('#featuredCarousel .carousel-indicators');
        this.carouselControlsContainer = document.getElementById('carousel-external-controls');

        this.perfilModal = document.getElementById('userProfileModal');
        this.itemModalElement = document.getElementById('itemModal');
        
        this.btnBuy = document.getElementById('btn-buy');
        if (this.btnBuy) this.btnBuy.addEventListener('click', () => this.handleCompraClick());
        
        this.btnDevolver = document.getElementById('btn-devolver');
        if (this.btnDevolver) this.btnDevolver.addEventListener('click', () => this.handleDevolucaoClick());
        
        // --- FILTROS LOJA ---
        this.shopTypeFilter = document.getElementById('shop-typeFilter');
        this.shopRarityFilter = document.getElementById('shop-rarityFilter');
        this.shopSearchInput = document.getElementById('shop-searchInput');
        // ... (Outros inputs de data/check mantidos)
        this.shopClearBtn = document.getElementById('shop-clearFilters');

        const shopInputs = ['shop-typeFilter', 'shop-rarityFilter', 'shop-searchInput', 'shop-checkNew', 'shop-checkForSale', 'shop-checkPromo'];
        shopInputs.forEach(id => {
            const el = document.getElementById(id);
            if(el) el.addEventListener(el.type === 'text' ? 'input' : 'change', () => this.renderizaItens());
        });

        if(this.shopClearBtn) this.shopClearBtn.addEventListener('click', () => this.limparFiltrosLoja());
        
        // --- FILTROS TODOS OS ITENS ---
        this.allItemsTab = document.getElementById('all-items-tab');
        this.allItemsGrid = document.getElementById('all-items-grid');
        this.allTypeFilter = document.getElementById('all-typeFilter');
        this.allRarityFilter = document.getElementById('all-rarityFilter');
        this.allSearchInput = document.getElementById('all-searchInput');
        this.allClearBtn = document.getElementById('all-clearFilters');

        const allInputs = ['all-typeFilter', 'all-rarityFilter', 'all-searchInput'];
        allInputs.forEach(id => {
            const el = document.getElementById(id);
            if(el) {
                if (el.type === 'text') {
                    let timeout;
                    el.addEventListener('input', () => { clearTimeout(timeout); timeout = setTimeout(() => this.buscaTodosOsItens(), 500); });
                } else {
                    el.addEventListener('change', () => this.buscaTodosOsItens());
                }
            }
        });

        if(this.allClearBtn) this.allClearBtn.addEventListener('click', () => this.limparFiltrosTodos());
        if (this.allItemsTab) this.allItemsTab.addEventListener('show.bs.tab', () => { if (!this.todosOsItensCarregados) this.buscaTodosOsItens(); });

        // Tabs de Usuário e Meus Itens
        this.usersTabContainer = document.getElementById('users-tab-container');
        this.myItemsTabContainer = document.getElementById('my-items-tab-container');
    }

    // --- DADOS E AUTH ---
    async verificaUsuario() {
        // Implementar lógica de auth quando o controller de usuários estiver pronto
        // Por enquanto, apenas exibe os menus se tiver token
        if (this.user) {
            if (this.myItemsTabContainer) this.myItemsTabContainer.style.display = 'block';
            // Atualizar navbar simulada
            this.navItens.innerHTML = `
                <span class="nav-creditos d-flex align-items-center me-3 text-light">Olá, Usuário</span>
                <button id="nav-logout" class="btn btn-sm btn-outline-danger">Sair</button>
            `;
            document.getElementById('nav-logout')?.addEventListener('click', () => {
                localStorage.removeItem('user_token'); // Ajuste conforme seu auth
                window.location.reload();
            });
        }
    }

    // ================================================================
    // BUSCA NA LOJA (Home)
    // ================================================================
    async buscaItensDisponiveis() {
        try {
            // ALTERAÇÃO 2: Endpoint relativo do seu Controller C#
            const response = await fetch(`${this.API_BASE_URL}/cosmeticos/loja`);
            
            if (!response.ok) throw new Error(`Erro HTTP: ${response.status}`);
            
            const itensDaApi = await response.json();
            
            if (Array.isArray(itensDaApi)) {
                // ALTERAÇÃO 3: Adaptação para o novo DTO aninhado (images.small, type.displayValue)
                this.itens = itensDaApi.map(item => {
                    const isAdquirido = this.itensAdquiridosSet.has(item.id);
                    return new ValidadorItem(item, isAdquirido).validaDados();
                });
                
                this.renderizaItens(); 
                this.preencheCarrousel(); 
            }
        } catch (error) {
            console.error('Erro ao buscar itens da loja:', error);
            this.mostrarErro('Erro ao carregar itens da loja.', this.cosmeticosGrid);
        }
    }

    // ================================================================
    // BUSCA TODOS OS ITENS (Catálogo)
    // ================================================================
    async buscaTodosOsItens() {
        if (!this.allItemsGrid) return;

        // Construção da URL com Query Params do C#
        const url = new URL(`${window.location.origin}${this.API_BASE_URL}/cosmeticos`);
        
        const busca = this.allSearchInput ? this.allSearchInput.value.trim() : '';
        const tipo = this.allTypeFilter ? this.allTypeFilter.value : '';
        const raridade = this.allRarityFilter ? this.allRarityFilter.value : '';

        // Mapeamento para os params do Controller C#
        if (busca) url.searchParams.append('nome', busca);
        if (tipo) url.searchParams.append('tipo', this._getTipoMapa('api')[tipo.toLowerCase()] || tipo);
        if (raridade) url.searchParams.append('raridade', this.traduzirRaridadeParaAPI(raridade));
        
        // ALTERAÇÃO 4: Paginação C# (Começa em 1, não 0)
        url.searchParams.append('page', '1'); 
        url.searchParams.append('pageSize', '100'); 

        try {
            this.allItemsGrid.innerHTML = this.gerarSpinner();
            
            const response = await fetch(url.toString());
            if (!response.ok) throw new Error(`Erro HTTP: ${response.status}`);
            
            const resultado = await response.json();
            
            // ALTERAÇÃO 5: O C# retorna { data: [...] }, não content
            const listaItensRaw = resultado.data || []; 

            this.todosOsItens = listaItensRaw.map(item => {
                const isAdquirido = this.itensAdquiridosSet.has(item.id);
                return new ValidadorItem(item, isAdquirido).validaDados();
            });

            this.renderizarTodosOsItens(); 
        } catch (error) {
            console.error('Erro catalogo:', error);
            this.mostrarErro('Erro ao carregar catálogo.', this.allItemsGrid);
        }
    }

    traduzirRaridadeParaAPI(val) {
        const mapa = { 'legendary': 'Lendário', 'epic': 'Épico', 'rare': 'Raro', 'uncommon': 'Incomum', 'common': 'Comum' };
        return mapa[val] || val;
    }

    // ================================================================
    // RENDERIZAÇÃO
    // ================================================================
    renderizaItens() {
        if (!this.cosmeticosGrid) return;
        
        // Filtros locais para a aba "Loja" (já que ela traz tudo de uma vez)
        const busca = this.shopSearchInput?.value.toLowerCase().trim();
        const tipo = this.shopTypeFilter?.value.toLowerCase();
        
        let itensFiltrados = this.itens.filter(item => {
            const matchNome = !busca || item.nome.toLowerCase().includes(busca);
            // Verifica se o tipo (ex: "Traje") contém o filtro (ex: "traje")
            const matchTipo = !tipo || item.tipo.toLowerCase().includes(this._getTipoMapa()[tipo] || tipo);
            return matchNome && matchTipo;
        });

        this.cosmeticosGrid.innerHTML = '';
        if (itensFiltrados.length === 0) {
           this.cosmeticosGrid.innerHTML = this.gerarHTMLVazio('Nenhum item na loja com estes filtros.');
           return;
        }

        const fragmento = document.createDocumentFragment();
        // Não precisamos agrupar bundles na visualização da loja oficial, mostra tudo
        for (const item of itensFiltrados) {
            fragmento.appendChild(this.criarCard(item));
        }
        this.cosmeticosGrid.appendChild(fragmento);
    }

    renderizarTodosOsItens() {
        if (!this.allItemsGrid) return;
        this.allItemsGrid.innerHTML = '';
        
        if (this.todosOsItens.length === 0) {
            this.allItemsGrid.innerHTML = this.gerarHTMLVazio('Nenhum item encontrado no catálogo.');
            return;
        }

        const fragmento = document.createDocumentFragment();
        for (const item of this.todosOsItens) {
            fragmento.appendChild(this.criarCard(item));
        }
        this.allItemsGrid.appendChild(fragmento);
    }

    // --- UI HELPERS ---
    gerarSpinner() {
        return `<div class="col-12 text-center py-5"><div class="spinner-border text-light" role="status"></div></div>`;
    }

    gerarHTMLVazio(msg) {
        return `<div class="col-12 text-center py-5 text-muted"><p>${msg}</p></div>`;
    }

    mostrarErro(msg, el) {
        if(el) el.innerHTML = `<div class="col-12 text-center py-5 text-danger"><p>${msg}</p></div>`;
    }

    preencheCarrousel() {
        if (!this.carrouselItems) return;
        // Filtra itens caros ou lendários para destaque
        const destaques = this.itens.filter(i => i.preco > 1500 || i.raridade.toLowerCase() === 'lendário').slice(0, 5);
        
        this.carrouselItems.innerHTML = ''; 
        if (this.carouselIndicators) this.carouselIndicators.innerHTML = ''; 

        if (destaques.length === 0) {
            this.alternarControlesCarrossel(false);
            return;
        }

        this.alternarControlesCarrossel(true);
        destaques.forEach((item, index) => {
            const div = document.createElement('div');
            div.className = `carousel-item ${index === 0 ? 'active' : ''}`;
            div.style.height = '500px'; 
            
            // Background gradiente baseado na raridade
            const corClass = `bg-rarity-${this.obterClasseRaridade(item.raridade)}`;
            
            div.innerHTML = `
                <div class="${corClass}" style="position: absolute; width:100%; height:100%; opacity: 0.8;"></div>
                <div class="carousel-caption">
                    <img src="${item.urlImagem}" style="height: 300px; object-fit: contain; margin-bottom: 20px; filter: drop-shadow(0 10px 10px rgba(0,0,0,0.5));">
                    <h3>${item.nome}</h3>
                    <p>${item.tipo} - ${item.preco} V-Bucks</p>
                </div>
            `;
            this.carrouselItems.appendChild(div);
            
            // Indicadores
            if (this.carouselIndicators) {
                const btn = document.createElement('button');
                btn.type = 'button';
                btn.dataset.bsTarget = '#featuredCarousel';
                btn.dataset.bsSlideTo = index;
                if (index === 0) btn.className = 'active';
                this.carouselIndicators.appendChild(btn);
            }
        });
    }

    alternarControlesCarrossel(show) {
        if(this.carouselControlsContainer) this.carouselControlsContainer.style.display = show ? 'flex' : 'none';
    }

    // --- CRIAÇÃO DE CARDS E MODAIS ---
    criarCard(item) {
        const col = document.createElement('div');
        col.className = 'col';
        const raridadeClass = this.obterClasseRaridade(item.raridade);
        
        col.innerHTML = `
            <div class="product-card h-100" style="cursor: pointer;">
                <div class="product-image bg-rarity-${raridadeClass} d-flex align-items-center justify-content-center" style="height: 250px; border-radius: 10px 10px 0 0;">
                    <img src="${item.urlImagem}" alt="${item.nome}" style="max-height: 90%; max-width: 90%; object-fit: contain;">
                </div>
                <div class="card-body p-3" style="background: #2a2a3d; border-radius: 0 0 10px 10px;">
                    <h5 class="text-light text-truncate">${item.nome}</h5>
                    <p class="text-muted small mb-1">${item.tipo}</p>
                    <div class="d-flex justify-content-between align-items-center">
                        <span class="badge bg-dark border border-secondary">${item.raridade}</span>
                        <span class="text-warning fw-bold">${item.preco} <img src="/images/vbuck.png" width="15" style="vertical-align: text-top;"></span>
                    </div>
                </div>
            </div>
        `;
        col.addEventListener('click', () => this.abrirModalItem(item));
        return col;
    }

    abrirModalItem(item) {
        if(!this.itemModalElement) return;
        
        // Preenche dados do modal
        document.getElementById('modal-item-name').textContent = item.nome;
        document.getElementById('modal-item-type').textContent = item.tipo;
        document.getElementById('modal-item-price').textContent = item.preco;
        document.getElementById('modal-item-description').textContent = item.descricao;
        document.getElementById('modal-detail-rarity').textContent = item.raridade;
        document.getElementById('modal-detail-category').textContent = item.tipo;
        document.getElementById('modal-detail-date').textContent = new Date(item.dataInclusao).toLocaleDateString();
        
        // Imagem e Fundo
        const imgContainer = document.getElementById('modal-item-image');
        imgContainer.className = `item-modal-image bg-rarity-${this.obterClasseRaridade(item.raridade)} d-flex justify-content-center align-items-center`;
        imgContainer.innerHTML = `<img src="${item.urlImagem}" style="max-height: 80%;">`;

        const modal = new bootstrap.Modal(this.itemModalElement);
        modal.show();
    }

    obterClasseRaridade(raridade) {
        if(!raridade) return 'common';
        const r = raridade.toLowerCase();
        if (r.includes('lendário') || r.includes('legendary')) return 'legendary';
        if (r.includes('épico') || r.includes('epic')) return 'epic';
        if (r.includes('raro') || r.includes('rare')) return 'rare';
        if (r.includes('incomum') || r.includes('uncommon')) return 'uncommon';
        if (r.includes('série') || r.includes('marvel') || r.includes('star wars')) return 'serie';
        return 'common';
    }

    // --- LIMPEZA ---
    limparFiltrosLoja() {
        if(this.shopSearchInput) this.shopSearchInput.value = '';
        if(this.shopTypeFilter) this.shopTypeFilter.value = '';
        if(this.shopRarityFilter) this.shopRarityFilter.value = '';
        this.renderizaItens();
    }

    limparFiltrosTodos() {
        if(this.allSearchInput) this.allSearchInput.value = '';
        if(this.allTypeFilter) this.allTypeFilter.value = '';
        if(this.allRarityFilter) this.allRarityFilter.value = '';
        this.buscaTodosOsItens();
    }
}

/**
 * Classe Helper para Normalizar os Dados da API C#
 * Transforma o DTO aninhado (images.small, type.displayValue) em um objeto plano p/ JS
 */
class ValidadorItem {
    constructor(apiData, isAdquirido) {
        this.raw = apiData;
        this.isAdquirido = isAdquirido;
    }

    validaDados() {
        // A API C# retorna DTOs com propriedades minúsculas devido ao JSON camelCase
        // Ex: id, name, description, type: { displayValue }, images: { small }
        
        return {
            id: this.raw.id || '',
            nome: this.raw.name || 'Sem Nome',
            // Acessa objetos aninhados com segurança (?.)
            tipo: this.raw.type?.displayValue || 'Cosmético',
            raridade: this.raw.rarity?.displayValue || 'Comum',
            // Tenta pegar imagem pequena, se não, icone, se não large
            urlImagem: this.raw.images?.small || this.raw.images?.icon || this.raw.images?.large || '',
            preco: this.raw.price || 0,
            descricao: this.raw.description || '',
            dataInclusao: this.raw.added || new Date().toISOString(),
            
            // Campos de controle
            isNew: false, // Pode implementar lógica de data se quiser
            isForSale: true,
            isAdquirido: this.isAdquirido,
            isBundle: false,
            cores: [] // A API do Fortnite não manda cores hex direto nesse endpoint simplificado
        };
    }
}