package ClassLib050;

public class Class001 {
    public static String property() {
        return "ClassLib050" + " " + ClassLib003.Class001.property() + " " + ClassLib007.Class001.property() + " " + ClassLib038.Class001.property() + " " + ClassLib012.Class001.property();
    }
}
